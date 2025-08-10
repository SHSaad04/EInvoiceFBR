$(document).ready(function () {
    let itemIndex = 0;

    const $productSelect = $("#InvoiceItem_Id");
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
    // Add Item
    //$("#addItemBtn").on("click", function () {
    //    const $form = $("#itemForm"); // form inside your modal

    //    // Re-parse validation rules in case modal is dynamically loaded
    //    $.validator.unobtrusive.parse($form);

    //    if (!$form.valid()) {
    //        // Validation errors will be shown automatically
    //        return;
    //    }

    //    const productName = $productSelect.find("option:selected").text();

    //    const row = `
    //    <tr>
    //        <td>${productName}<input type="hidden" name="InvoiceItems[${itemIndex}].ProductId" value="${$productSelect.val()}" /></td>
    //        <td>${$quantityInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].Quantity" value="${$quantityInput.val()}" /></td>
    //        <td>${$rateInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].Rate" value="${$rateInput.val()}" /></td>
    //        <td>${$totalValueInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].TotalValue" value="${$totalValueInput.val()}" /></td>
    //        <td><button type="button" class="btn btn-sm btn-danger removeItem">X</button></td>
    //    </tr>
    //`;

    //    $invoiceItemsTableBody.append(row);
    //    itemIndex++;

    //    $("#itemModal").modal("hide");
    //    clearItemForm();
    //});

    //// Add Item
    //$("#addItemBtnOld").on("click", function () {
    //    if (!$productSelect.val()) {
    //        alert("Please select a product");
    //        return;
    //    }

    //    const productName = $productSelect.find("option:selected").text();

    //    const row = `
    //        <tr>
    //            <td>${productName}<input type="hidden" name="InvoiceItems[${itemIndex}].ProductId" value="${$productSelect.val()}" /></td>
    //            <td>${$quantityInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].Quantity" value="${$quantityInput.val()}" /></td>
    //            <td>${$rateInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].Rate" value="${$rateInput.val()}" /></td>
    //            <td>${$totalValueInput.val()}<input type="hidden" name="InvoiceItems[${itemIndex}].TotalValue" value="${$totalValueInput.val()}" /></td>
    //            <td><button type="button" class="btn btn-sm btn-danger removeItem">X</button></td>
    //        </tr>
    //    `;

    //    $invoiceItemsTableBody.append(row);
    //    itemIndex++;

    //    // Close modal
    //    $("#itemModal").modal("hide");

    //    clearItemForm();
    //});

    // Remove Item
    $invoiceItemsTableBody.on("click", ".removeItem", function () {
        $(this).closest("tr").remove();
    });

    function clearItemForm() {
        $("#product, #hsCode, #uom, #rate, #totalValue, #valueSalesExcludingST").val("");
        $quantityInput.val(1);
    }
});
