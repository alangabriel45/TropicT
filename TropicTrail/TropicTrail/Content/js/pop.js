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
        $("#confirmReservationBtn").data("id", reservationId);
        $("#confirmModal").modal("show");
    });

    $("#confirmReservationBtn").click(function () {
        var reservationId = $(this).data("id");
        
        $.ajax({
            url: "/Admin/ManageReservation", 
            type: "POST",
            data: { id: reservationId },
            success: function (response) {
                
                location.reload();
            },
            error: function (xhr, status, error) {
                
                console.error(error);
            }
        });
    });
});

function searchTransactions() {
    var input, filter, table, tr, td, i, txtValue;
    input = document.getElementById("searchInput");
    filter = input.value.toUpperCase();
    table = document.getElementById("transactionTable");
    tr = table.getElementsByTagName("tr");
    for (i = 0; i < tr.length; i++) {
        var reservationIdCell = tr[i].getElementsByTagName("td")[0]; // First cell for Reservation ID
        var bookedByCell = tr[i].getElementsByTagName("td")[1]; // Second cell for Booked By
        if (reservationIdCell || bookedByCell) {
            var reservationIdText = reservationIdCell.textContent || reservationIdCell.innerText;
            var bookedByText = bookedByCell.textContent || bookedByCell.innerText;
            if (reservationIdText.toUpperCase().indexOf(filter) > -1 || bookedByText.toUpperCase().indexOf(filter) > -1) {
                tr[i].style.display = "";
            } else {
                tr[i].style.display = "none";
            }
        }
    }
}
function openPopup() {
    var popup = document.getElementById("popupSquare");
    popup.style.display = "block";
}

function closePopup() {
    var popup = document.getElementById("popupSquare");
    popup.style.display = "none";
}

function sumReservationsAndPayments() {
    var dropdown = document.getElementById("totalDropdown");
    var selectedOption = dropdown.options[dropdown.selectedIndex].value;
    var total = 0;

    if (selectedOption === "totalReservation") {
        // Calculate total number of rows (excluding header)
        total = document.querySelectorAll("#transactionTable tbody tr").length - 1;
    } else if (selectedOption === "totalIncome") {
        // Calculate total income as before
        var rows = document.querySelectorAll("#transactionTable tbody tr");
        rows.forEach(function (row) {
            var payment = parseFloat(row.cells[4].innerText);
            if (!isNaN(payment)) {
                total += payment;
            }
        });
    }

    document.getElementById("outputTextBox").value = total;
}
function previewProfilePicture(event) {
    var input = event.target;
    var reader = new FileReader();
    reader.onload = function () {
        var img = document.getElementById('profilePicturePreview');
        img.src = reader.result;
        img.style.display = 'block';
    };
    reader.readAsDataURL(input.files[0]);
}