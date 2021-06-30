namespace Fanword.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SeedKeywordType : DbMigration
    {
        public override void Up()
        {
			Sql(@"INSERT INTO RssKeywordTypes
(Id, Name, IsActive)
VALUES
(NEWID(), 'Contains', 1)");
		}
        
        public override void Down()
        {
        }
    }
}
