let popup = document.getElementById("pop-up");
let username = document.getElementById("userName");
let password = document.getElementById("password");
let confirmPass = document.getElementById("confirmPass");
let email = document.getElementById("email");

if (username != null && password != null && confirmPass != null && email != null) {
    function openPopup() {
        popup.classList.add("open-popup");
    }
    function closePopup() {
        popup.classList.remove("open-popup");
    }
}
