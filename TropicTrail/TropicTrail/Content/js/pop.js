$(document).ready(function () {
    // Event listener for the confirm button in the modal
    $("#confirmFinish").click(function () {
        // Get the selected rating
        var rating = $("#rating").val();

        // Get the reservation ID
        var reservationId = $(".btn-finish").data("id");

        // AJAX request to the Finish action with reservation ID and ratings
        $.ajax({
            url: "/Home/Finish", // Replace YourController with your actual controller name
            type: "POST",
            data: { id: reservationId, rating: rating }, // Pass reservation ID and ratings
            success: function (response) {
                // Handle success, such as redirecting to YourReservation page
                window.location.href = "/Home/YourReservation"; // Replace YourController with your actual controller name
            },
            error: function (xhr, textStatus, errorThrown) {
                // Handle error
                console.log("Error: " + errorThrown);
            }
        });
    });
});

$(document).ready(function () {
    $(".btn-confirm").click(function () {
        var reservationId = $(this).data("id");
        $("#confirmReservationBtn").data("id", reservationId); // Set reservation ID to the Confirm button
        $("#confirmModal").modal("show"); // Show the confirmation modal
    });

    $("#confirmReservationBtn").click(function () {
        var reservationId = $(this).data("id");
        // AJAX call to confirm reservation
        $.ajax({
            url: "/Admin/ManageReservation", // Replace YourController with your actual controller name
            type: "POST",
            data: { id: reservationId },
            success: function (response) {
                // Reload page or update UI as needed
                location.reload(); // Example: reload the page
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error(error);
            }
        });
    });
});