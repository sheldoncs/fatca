$(document).ready(function () { 

  $("#btnStats").on("click",function(){
     
     var ctx = document.getElementById('myChart').getContext('2d');
     $("#datacharts").show();
     $("#toolbox").hide();

     var chart = new Chart(ctx, {
    // The type of chart we want to create
        type: 'doughnut',

        // The data for our dataset
        data: {
            labels: ["January", "February", "March", "April", "May", "June", "July"],
            datasets: [{
                label: "My First dataset",
                data: [0, 10, 5, 2, 20, 30, 45],
                backgroundColor: ["#3e95cd", "#8e5ea2","#3cba9f","#e8c3b9","#c45850","#386592","#FC9905"]
            }]
        },

    // Configuration options go here
      options: {}
   });


  });

    


});