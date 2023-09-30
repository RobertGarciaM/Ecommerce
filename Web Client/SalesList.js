document.addEventListener("DOMContentLoaded", function () {
    const searchButton = document.getElementById("searchButton");
    const startDateInput = document.getElementById("startDate");
    const endDateInput = document.getElementById("endDate");
    const salesList = document.getElementById("salesList");

    searchButton.addEventListener("click", async () => {
        const startDate = startDateInput.value;
        const endDate = endDateInput.value;

        if (!startDate || !endDate) {
            alert("Please select a start date and an end date.");
            return;
        }

        const token = localStorage.getItem("token");

        if (!token) {
            alert("You have not logged in or the token is not available.");
            return;
        }

        const headers = {
            "Authorization": `Bearer ${token}`,
            "Content-Type": "application/json"
        };

        try {
            const response = await fetch(`https://localhost:44349/api/sale/GetSalesByDate?startDate=${startDate}&endDate=${endDate}`, {
                method: "GET",
                headers: headers
            });

            if (!response.ok) {
                throw new Error("Error searching for sales.");
            }

            const sales = await response.json();
            salesList.innerHTML = "";

            if (sales.length === 0) {
                salesList.innerHTML = "<p>No sales were found in the specified date range.</p>";
            } else {
                sales.forEach((sale) => {
                    const listItem = document.createElement("li");
                    listItem.textContent = `Customer : ${sale.Customer}, Product: ${sale.Product}, Quantity: ${sale.Quantity}, Date: ${sale.Date.slice(0, 10)}`;
                    salesList.appendChild(listItem);
                });
            }
        } catch (error) {
            console.error(error);
            alert("An error occurred while searching for sales.");
        }
    });
});
