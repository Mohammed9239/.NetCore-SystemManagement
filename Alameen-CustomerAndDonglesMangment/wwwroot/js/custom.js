
"use strict";

$(document).ready(function () {
  /*-- sidebar js --*/
  $('#sidebarCollapse').on('click', function () {
    $('#sidebar').toggleClass('active');
  });

  /*-- tooltip js --*/
  $('[data-toggle="tooltip"]').tooltip();

  function net(){
    var s= navigator.onLine;
    if(s){
      $('#online').show();
      $('#off').hide();
    } else{
      $('#online').hide();
      $('#off').show();
    }
  }

  net();

  
});
