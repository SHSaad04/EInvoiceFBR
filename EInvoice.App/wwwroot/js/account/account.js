$(document).ready(function () {
    function logoutUser() {
        AppAjax.post(AppConfig.apiBaseUrlUser + "/Signoff", {}, function (res) {
            // Remove token from local storage
            localStorage.removeItem("jwtToken");

            alert("You have been logged out.");
            window.location.href = "/Users/Login";
        });
    }

});
