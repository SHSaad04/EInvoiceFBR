$(document).ready(function () {
    let itemIndex = 0;

    const $productSelect = $("#InvoiceItem_ProductId");
    const $quantityInput = $("#InvoiceItem_Quantity");
    const $retailPrice = $("#InvoiceItem_FixedNotifiedValueOrRetailPrice");
    const $rateInput = $("#InvoiceItem_Rate");

    const $totalValueInput = $("#InvoiceItem_TotalValue");
    const $valueSalesExcludingSTInput = $("#InvoiceItem_ValueSalesExcludingST");
    const $invoiceItemsTableBody = $("#InvoiceItem_InvoiceItems");

    $productSelect.on("change", function () {
        const productId = $(this).val();
        if (!productId) return;

        $.ajax({
            url: '/Product/GetProduct/' + productId,
            type: 'GET',
            dataType: 'json',
            success: function (data) {
                if (data) {
                    $("#ProductDescription").val(data.ProductDescription);
                    $("#InvoiceItem_HsCode").val(data.HsCode);
                    $("#InvoiceItem_UoM").val(data.UoM);
                    $("#InvoiceItem_Rate").val(data.Rate);
                    $("#InvoiceItem_FixedNotifiedValueOrRetailPrice").val(data.FixedNotifiedValueOrRetailPrice);
                    $quantityInput.val(1);
                    calculateTotals();
                }
                else {
                    $quantityInput.val(0);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching product:", error);
                alert("Failed to load product details.");
            }
        });
    });

    // Quantity change event
    $quantityInput.on("input", calculateTotals);

    function calculateTotals() {
        const qty = parseFloat($quantityInput.val()) || 0;
        const price = parseFloat($retailPrice.val()) || 0;
        const taxRate = parseFloat($rateInput.val()) || 0;

        const subTotal = qty * price; // Before tax
        const taxAmount = (subTotal * taxRate) / 100;
        const total = subTotal + taxAmount; // Including tax

        $totalValueInput.val(total.toFixed(2));              // With tax
        $valueSalesExcludingSTInput.val(subTotal.toFixed(2)); // Without tax
    }

    // Remove Item
    $invoiceItemsTableBody.on("click", ".removeItem", function () {
        $(this).closest("tr").remove();
    });

    function clearItemForm() {
        $("#product, #hsCode, #uom, #rate, #totalValue, #valueSalesExcludingST").val("");
        $quantityInput.val(1);
    }
    $('#Invoice-form').on('submit', function () {
        $('#InvoiceItemsJson').val(JSON.stringify(invoiceItems));
    });
});
