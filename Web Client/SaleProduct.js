document.addEventListener("DOMContentLoaded", function () {
  const saleForm = document.getElementById("sale-form");
  const resultDiv = document.getElementById("result");

  saleForm.addEventListener("submit", function (e) {
    e.preventDefault();
    const customerId = document.getElementById("customerId").value;
    const productId = document.getElementById("productId").value;
    const quantity = document.getElementById("quantity").value;
    const date = document.getElementById("date").value;
    const token = localStorage.getItem("token");

    const saleData = {
      IdCustomer: customerId,
      IdProduct: productId,
      Quantity: quantity,
      Date: date,
    };

    fetch("https://localhost:44349/api/sale/Create", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(saleData),
    })
      .then((response) => {
        if (response.ok) {
          return response.json();
        } else {
          throw new Error("Error creating sale.");
        }
      })
      .then((data) => {
        window.location.href = "SalesList.html";
      })
      .catch((error) => {
        resultDiv.textContent = "Error: " + error.message;
      });
  });
});
