$(document).ready(function () {
    $("#btnLogin").click(function () {
        var credentials = {
            username: $("#username").val(),
            password: $("#password").val()
        };

        apiRequest(AppConfig.HttpMethod.POST, AppConfig.apiBaseUrlUser + "/authenticate", credentials,
            function (res) {
                localStorage.setItem("jwtToken", res.token);
                alert("Login successful!");
                window.location.href = "/Home/Index"; // adjust your post-login page
            },
            function (err) {
                localStorage.removeItem("jwtToken");
                alert("Login failed: " + err.responseText);
            }
        );
    });
});
