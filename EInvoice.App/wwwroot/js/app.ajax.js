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
            if (response.success && onSuccess) {
                onSuccess(response);
            }
        },
        error: function (xhr) {
            if (xhr.status === 401) {
                alert("Your session has expired. Please login again.");
                window.location.href = "/Account/Login"; // adjust path
            } else if (onError) {
                onError(xhr);
            } else {
                console.error("API Error:", xhr);
            }
        }
    });
}
//Sweet Alert
$(document).on('click', '.delete-confirm', function (e) {
    e.preventDefault(); // Stop the default navigation

    const url = $(this).data('url');

    Swal.fire({
        title: 'Are you sure?',
        text: "This action cannot be undone!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#6c757d',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            // Proceed to the delete URL
            window.location.href = url;
        }
    });
});
