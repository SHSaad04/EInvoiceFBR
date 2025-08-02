// app.ajax.js
function apiRequest(method, endpoint, data, onSuccess, onError) {
    let token = localStorage.getItem("jwtToken");

    $.ajax({
        url: endpoint,
        type: method,
        contentType: "application/json",
        data: data ? JSON.stringify(data) : null,
        headers: token ? { "Authorization": "Bearer " + token } : {},
        success: function (response) {
            if (onSuccess) onSuccess(response);
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                alert("Your session has expired. Please login again.");
                window.location.href = "/Account/Login"; // adjust path
            } else {
                if (onError) onError(xhr);
                else console.error("API Error:", xhr);
            }
        }
    });
}
