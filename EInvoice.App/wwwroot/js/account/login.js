$(document).ready(function () {
    $("#btnLogin").click(function () {
        var credentials = {
            username: $("#username").val(),
            password: $("#password").val()
        };

        apiRequest(AppConfig.HttpMethod.POST, AppConfig.apiBaseUrlUser + "/authenticate", credentials,
            function (res) {
                alert("Login successful!");
                window.location.href = "/Organization/Index"; // adjust your post-login page
            },
            function (err) {
                alert("Login failed: " + err.responseText);
            }
        );
    });
});
