var Login = function () {

	var handleLogin = function() {
	
	}

	var handleForgetPassword = function () {
	

	        jQuery('#forget-password').click(function () {
	            jQuery('.login-form').hide();
	            jQuery('.forget-form').show();
	        });

	        jQuery('#back-btn').click(function () {
	            jQuery('.login-form').show();
	            jQuery('.forget-form').hide();
	        });

	}

	var handleRegister = function () {

		        function format(state) {
            if (!state.id) { return state.text; }
            var $state = $(
             '<span><img src="/Theme/Metronic/assets/global/img/flags/' + state.element.value.toLowerCase() + '.png" class="img-flag" /> ' + state.text + '</span>'
            );
            
            return $state;
        }

        


       

	        jQuery('#register-btn').click(function () {
	            jQuery('.login-form').hide();
	            jQuery('.register-form').show();
	        });

	        jQuery('#register-back-btn').click(function () {
	            jQuery('.login-form').show();
	            jQuery('.register-form').hide();
	        });
	}
    
    return {
        //main function to initiate the module
        init: function () {
        	
            handleLogin();
            handleForgetPassword();
            handleRegister();    

            // init background slide images
		    $.backstretch([
		        "/Theme/Metronic/assets/pages/media/bg/1.jpg",
		        "/Theme/Metronic/assets/pages/media/bg/2.jpg",
		        "/Theme/Metronic/assets/pages/media/bg/3.jpg",
		        "/Theme/Metronic/assets/pages/media/bg/4.jpg"
		        ], {
		          fade: 1000,
		          duration: 8000
		    	}
        	);
        }
    };

}();

jQuery(document).ready(function() {
    Login.init();
});