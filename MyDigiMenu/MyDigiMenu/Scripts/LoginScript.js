$(document).ready(function () {

    $("#toggle-password").click(function () {
        let passwordField = $("#password");
        let type = passwordField.attr("type") === "password" ? "text" : "password";
        passwordField.attr("type", type);

        // Change button text/icon
        $(this).text(type === "password" ? "👁️" : "🙈");
    });

    // Handle login form submission
    $('#login-form').on('submit', function (e) {
        e.preventDefault();

        // Show loading spinner
        $('#loading-spinner').show();

        // Get form data
        var username = $('#username').val();
        var password = $('#password').val();

        $.ajax({
            url: 'Account/Login',
            method: 'POST',
            data: {
                username: username,
                password: password
            },
            success: function (response) {

                $('#loading-spinner').hide();

                if (response.success) {

                    if (response.admin) {
                        window.location.href = '/User/All';
                    } else {
                        window.location.href = '/Recipe/All';
                    }
                } else {
                    // Show failure notification
                    showNotification('Login failed: ' + response.message, 'error');
                }
            },
            error: function () {
                // Hide loading spinner
                $('#loading-spinner').hide();
                // Show generic error notification
                showNotification('An error occurred while processing your request.', 'error');
            }
        });
    });

    // Function to show notification
    function showNotification(message, type) {
        // Set the notification message
        $('#notification-message').text(message);

        // Add class based on the type of notification (success or error)
        if (type === 'error') {
            $('#notification').css('background-color', '#e74c3c'); // Red for errors
        } else {
            $('#notification').css('background-color', '#2ecc71'); // Green for success
        }

        // Show the notification with a fade-in effect
        $('#notification').addClass('show');

        // Hide the notification after 5 seconds
        setTimeout(function () {
            $('#notification').removeClass('show');
        }, 5000); // 5 seconds
    }   

});