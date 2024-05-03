let submit = document.getElementById("submit");
let username = document.getElementById("userName");
let password = document.getElementById("password");
let confirmPass = document.getElementById("confirmPass");
let email = document.getElementById("email");

submit.addEventListener('onclick', () => {
    if (username != null && password != null && confirmPass != null && email != null) {
        function openPopup() {
            popup.classList.add("open-popup");
        }
    }
});


