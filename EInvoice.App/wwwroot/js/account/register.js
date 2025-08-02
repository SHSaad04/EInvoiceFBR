$(document).ready(function () {
    $("#btnRegister").click(function () {
        var user = {
            userName: $("#username").val(),
            email: $("#email").val(),
            password: $("#password").val(),
            firstName: $("#firstName").val(),
            lastName: $("#lastName").val(),
            contact: $("#contact").val(),
            country: $("#country").val(),
            city: $("#city").val(),
            promoCode: $("#promoCode").val()
        };

        apiRequest(AppConfig.HttpMethod.POST, AppConfig.apiBaseUrlUser + "/signup", user,
            function (res) {
                alert("Registration successful! Please login.");
                window.location.href = "/Account/Login";
            },
            function (err) {
                alert("Registration failed: " + err.responseText);
            }
        );
    });
});
