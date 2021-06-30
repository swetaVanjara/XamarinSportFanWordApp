using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Description;
using Fanword.Api.Models;
using FileUploads.Azure;
using FileUploads.Azure.Models;
using FileUploads.Core.Models;

namespace Fanword.Api.Controllers
{
    [Authorize, RoutePrefix("api/Uploads")]
    public class UploadsController : ApiController
    {
        [Route("Part"), HttpPost, ResponseType(typeof(string))]
        public async Task<IHttpActionResult> Part()
        {
            FileStream stream = null;
            try
            {
                var localTemp = HostingEnvironment.MapPath("~/Uploads/Temp");
                CreateDirectory(localTemp);
                var requestStream = await Request.Content.ReadAsStreamAsync();
                var fileName = Request.Headers.FirstOrDefault(m => m.Key == "Filename");
                if (fileName.Value == null) return BadRequest("No File Name");
                stream = File.Create(Path.Combine(localTemp, fileName.Value.First()), (int)requestStream.Length);
                var bytesInStream = new byte[requestStream.Length];
                requestStream.Read(bytesInStream, 0, bytesInStream.Length);
                await stream.WriteAsync(bytesInStream, 0, bytesInStream.Length);
                stream.Close();
                return Ok(fileName.Value.First());
            }
            catch (Exception ex)
            {
                if (stream != null)
                {
                    try
                    {
                        stream.Close();
                    }
                    catch (Exception)
                    {

                    }
                }
                return BadRequest("Error Uploading Part");
            }
        }

        [HttpPost, Route("UserProfilePhoto")]
        public async Task<IHttpActionResult> UserProfilePhoto(RebuildFile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var azureFileData = await RebuildFile(model, "userprofiles");
            if (azureFileData == null)
                return BadRequest("Missing files to rebuild");

            return Ok(azureFileData);
        }

        [HttpPost, Route("PostPhoto")]
        public async Task<IHttpActionResult> PostPhoto(RebuildFile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var azureFileData = await RebuildFile(model, "posts");
            if (azureFileData == null)
                return BadRequest("Missing files to rebuild");

            return Ok(azureFileData);
        }

        [HttpPost, Route("PostVideo")]
        public async Task<IHttpActionResult> PostVideo(RebuildFile model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var azureFileData = await RebuildFile(model, "posts");
            if (azureFileData == null)
                return BadRequest("Missing files to rebuild");

            return Ok(azureFileData);
        }

        private void CleanUpLocalStorage(RebuildFile model, string rebuiltMapped, string newFileName, string localTemp)
        {
            //var time = new System.Timers.Timer { Interval = 5000 };
            //time.Elapsed += (o, eventArgs) => {
            if (File.Exists(Path.Combine(rebuiltMapped, newFileName)))
            {
                File.Delete(Path.Combine(rebuiltMapped, newFileName));
            }
            foreach (var file in model.Guids.Where(file => File.Exists(Path.Combine(localTemp, file))))
            {
                File.Delete(Path.Combine(localTemp, file));
            }
            //time.Stop();
            //};
            //time.Start();
        }

        private void CreateDirectory(string localTemp)
        {
            if (!Directory.Exists(localTemp))
            {
                Directory.CreateDirectory(localTemp);
            }
        }

        private async Task<AzureFileData> RebuildFile(RebuildFile model, string container)
        {
            var rebuiltMapped = HostingEnvironment.MapPath("~/Uploads/Temp/Rebuilt");
            var localTemp = HostingEnvironment.MapPath("~/Uploads/Temp");
            CreateDirectory(localTemp);
            CreateDirectory(rebuiltMapped);
            var newFile = String.Format("file_{0}{1}", Guid.NewGuid().ToString(), model.FileExtension);
            var allFilesExists = model.Guids.All(filePart => File.Exists(Path.Combine(localTemp, filePart)));
            if (!allFilesExists) return null;

            using (var streamWriter = new StreamWriter(Path.Combine(rebuiltMapped, newFile)))
            {
                foreach (var filePart in model.Guids)
                {
                    using (var streamReader = new StreamReader(Path.Combine(localTemp, filePart)))
                    {
                        var fileSize = (int)streamReader.BaseStream.Length;
                        var fileBytes = new byte[fileSize];
                        await streamReader.BaseStream.ReadAsync(fileBytes, 0, fileSize);
                        await streamWriter.BaseStream.WriteAsync(fileBytes, 0, fileSize);
                    }
                }
            }

            var data = new AzureStorage(container, true).SaveFile(Path.Combine(rebuiltMapped, newFile));
            CleanUpLocalStorage(model, rebuiltMapped, newFile, localTemp);
            return data;
        }
    }
}
