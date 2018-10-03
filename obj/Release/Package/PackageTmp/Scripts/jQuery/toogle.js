$(document).ready(function () {

    /* Stats Toogle */
    $("#open-stats, #stats .close").click(function(){
        $("#stats").slideToggle()
    });


    $(".simple-tips .close").click(function(){
        $(".simple-tips").slideToggle()
    });


    /* ALERT AND DIALOG BOXES */ 
    $(".albox .close").click(function(event){
	    $(this).parents(".albox").slideToggle();
		return false;
    });
 
 
	$(".toggle-message .title, .toggle-message p").click(function(event){
		$(this).parents(".toggle-message").find(".hide-message").slideToggle();
		return false;
	});
 

    /* SUBMENU */
	$(".subtitle .action").click(function(event){
		$(this).parents(".subtitle").find(".submenu").slideToggle();
		return false;
    });

});