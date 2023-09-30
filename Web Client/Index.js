document.addEventListener("DOMContentLoaded", function () {
    const loginForm = document.getElementById("login-form");
    const loginMessage = document.getElementById("login-message");

    loginForm.addEventListener("submit", function (e) {
        e.preventDefault();

        const username = document.getElementById("username").value;
        const password = document.getElementById("password").value;
        
        const credentials = {
            UserName: username,
            Password: password
        };

        const requestOptions = {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify(credentials)
        };

        fetch("https://localhost:44349/api/Account/Login", requestOptions)
            .then(response => {
                if (response.ok) {
                    response.json().then(data => {
                        localStorage.setItem("token", data.Token);
                    });

                    window.location.href = "SalesList.html";
                } else {
                    // Si la respuesta es un error, muestra un mensaje de error.
                    loginMessage.textContent = "Incorrect credentials. Please try again.";
                }
            })
            .catch(error => {
                console.error("Network error", error);
                loginMessage.textContent = "Network error. Please try again later.";
            });
    });
});
