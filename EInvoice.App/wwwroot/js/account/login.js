$(document).ready(function () {
    $("#btnLogin").click(function () {
        var credentials = {
            username: $("#username").val(),
            password: $("#password").val()
        };

        apiRequest(AppConfig.HttpMethod.POST, AppConfig.apiBaseUrlUser + "/authenticate", credentials,
            function (res) {
                if (res.isOrganizationAssociated) {
                    window.location.href = "/Organization/Index";
                } else {
                    window.location.href = "/Organization/Add";
                }
            },
            function (err) {
                alert("Login failed: " + err.responseText);
            }
        );
    });
});
