$(function() {

    "use strict";

    $(document).on("click", ".box-body", function () {
        var clickedBtnID = $(this).attr('id'); // or var clickedBtnID = this.id
        alert('you clicked on button #' + clickedBtnID);
    });
});
