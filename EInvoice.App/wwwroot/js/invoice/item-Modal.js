$(function () {
    const itemModal = new bootstrap.Modal($("#itemModal")[0]);
    const form = $("#itemForm");

    // Add Item
    $("#btnAddItem").on("click", function () {
        form[0].reset();
        itemModal.show();
    });

    // Edit Item (when clicking edit buttons in the table)
    //$(document).on("click", ".edit-item", function () {
    //    fillModal($(this).data());
    //    itemModal.show();
    //});

    function clearModal() {
        $("#ProductId").val("");
        $("#ProductName").val("");
        $("#ProductRate").val("");
        // Clear other fields if needed
    }

    //function fillModal(data) {
    //    $("#ProductId").val(data.id);
    //    $("#ProductName").val(data.name);
    //    $("#ProductRate").val(data.rate);
    //    // Map other fields if needed
    //}

    //Add Item
    //let invoiceItems = [];
    let editIndex = null;
    $("#addItemBtn").on("click", function (e) {
        e.preventDefault(); // prevent default button action if it's in a form

        // Check if the form is valid using jQuery Validate or native checkValidity

        // Trigger unobtrusive validation manually
        if (!form.valid()) {
            return; // Stop if validation fails
        }
        const newItem = {
            ProductId: $("#InvoiceItem_Id").val(),
            HsCode: $("#InvoiceItem_HsCode").val(),
            ProductDescription: $("#ProductDescription").val(),
            Rate: parseFloat($("#InvoiceItem_Rate").val()) || 0,
            UoM: $("#InvoiceItem_UoM").val(),
            Quantity: parseFloat($("#InvoiceItem_Quantity").val()) || 0,
            TotalValue: parseFloat($("#InvoiceItem_TotalValue").val()) || 0,
            ValueSalesExcludingST: parseFloat($("#InvoiceItem_ValueSalesExcludingST").val()) || 0,
            FixedNotifiedValueOrRetailPrice: parseFloat($("#InvoiceItem_FixedNotifiedValueOrRetailPrice").val()) || 0,
            SalesTaxApplicable: parseFloat($("#InvoiceItem_SalesTaxApplicable").val()) || 0,
            SalesTaxWithheldAtSource: parseFloat($("#InvoiceItem_SalesTaxWithheldAtSource").val()) || 0,
            ExtraTax: parseFloat($("#InvoiceItem_ExtraTax").val()) || 0,
            FurtherTax: parseFloat($("#InvoiceItem_FurtherTax").val()) || 0,
            SroScheduleNo: $("#InvoiceItem_SroScheduleNo").val() || "",
            FedPayable: parseFloat($("#InvoiceItem_FedPayable").val()) || 0,
            Discount: parseFloat($("#InvoiceItem_Discount").val()) || 0,
            SaleType: $("#InvoiceItem_SaleType").val(),
            SroItemSerialNo: $("#InvoiceItem_SroItemSerialNo").val() || ""
        };

        if (editIndex !== null) {
            // Update existing item
            let existing = invoiceItems[editIndex];
            // Keep ProductDescription from the existing item
            newItem.ProductDescription = existing.ProductDescription;
            invoiceItems[editIndex] = newItem;
            editIndex = null;
            $('#addItemBtn').text('Add Item');
        } else {
            // Add new item
            invoiceItems.push(newItem);
        }
        renderInvoiceCards();

        $("#itemModal").modal("hide");
    });

    // Render cards
    function renderInvoiceCards() {
        const template = $('#invoiceItemCardTemplate').html();
        const $container = $('#invoiceItemsCards');
        $container.empty();

        invoiceItems.forEach((item, index) => {
            let cardHtml = template
                .replace(/{ProductDescription}/g, item.ProductDescription)
                .replace(/{HsCode}/g, item.HsCode)
                .replace(/{Quantity}/g, item.Quantity)
                .replace(/{UoM}/g, item.UoM)
                .replace(/{Rate}/g, item.Rate.toFixed(2))
                .replace(/{TotalValue}/g, item.TotalValue.toFixed(2))
                .replace(/{Index}/g, index);

            // wrap card and hidden fields together
            const $wrapper = $('<div>').append(cardHtml);
            $wrapper.append(`
            <input type="hidden" name="InvoiceItems[${index}].ProductId" value="${item.ProductId}" />
            <input type="hidden" name="InvoiceItems[${index}].HsCode" value="${item.HsCode}" />
            <input type="hidden" name="InvoiceItems[${index}].ProductDescription" value="${item.ProductDescription}" />
            <input type="hidden" name="InvoiceItems[${index}].Rate" value="${item.Rate}" />
            <input type="hidden" name="InvoiceItems[${index}].UoM" value="${item.UoM}" />
            <input type="hidden" name="InvoiceItems[${index}].Quantity" value="${item.Quantity}" />
            <input type="hidden" name="InvoiceItems[${index}].TotalValue" value="${item.TotalValue}" />
            <input type="hidden" name="InvoiceItems[${index}].ValueSalesExcludingST" value="${item.ValueSalesExcludingST}" />
            <input type="hidden" name="InvoiceItems[${index}].FixedNotifiedValueOrRetailPrice" value="${item.FixedNotifiedValueOrRetailPrice}" />
            <input type="hidden" name="InvoiceItems[${index}].SalesTaxApplicable" value="${item.SalesTaxApplicable}" />
            <input type="hidden" name="InvoiceItems[${index}].SalesTaxWithheldAtSource" value="${item.SalesTaxWithheldAtSource}" />
            <input type="hidden" name="InvoiceItems[${index}].ExtraTax" value="${item.ExtraTax}" />
            <input type="hidden" name="InvoiceItems[${index}].FurtherTax" value="${item.FurtherTax}" />
            <input type="hidden" name="InvoiceItems[${index}].SroScheduleNo" value="${item.SroScheduleNo}" />
            <input type="hidden" name="InvoiceItems[${index}].FedPayable" value="${item.FedPayable}" />
            <input type="hidden" name="InvoiceItems[${index}].Discount" value="${item.Discount}" />
            <input type="hidden" name="InvoiceItems[${index}].SaleType" value="${item.SaleType}" />
            <input type="hidden" name="InvoiceItems[${index}].SroItemSerialNo" value="${item.SroItemSerialNo}" />
        `);

            $container.append($wrapper);
        });
    }

    // Remove Item
    $(document).on("click", ".remove-item", function () {
        const index = $(this).data("index");
        invoiceItems.splice(index, 1);
        renderInvoiceCards();
    });

    // Edit Item

    $(document).on("click", ".edit-item", function () {
        editIndex = $(this).data('index');
        const item = invoiceItems[editIndex];
        // populate modal fields with item values
        $("#InvoiceItem_Id").val(item.ProductId);
        $("#InvoiceItem_ProductId").val(item.ProductId);
        $("#InvoiceItem_HsCode").val(item.HsCode);
        $("#InvoiceItem_ProductDescription").val(item.ProductDescription);
        $("#InvoiceItem_Rate").val(item.Rate);
        $("#InvoiceItem_UoM").val(item.UoM);
        $("#InvoiceItem_Quantity").val(item.Quantity);
        $("#InvoiceItem_TotalValue").val(item.TotalValue);
        $("#InvoiceItem_ValueSalesExcludingST").val(item.ValueSalesExcludingST);
        $("#InvoiceItem_FixedNotifiedValueOrRetailPrice").val(item.FixedNotifiedValueOrRetailPrice);
        $("#InvoiceItem_SalesTaxApplicable").val(item.SalesTaxApplicable);
        $("#InvoiceItem_SalesTaxWithheldAtSource").val(item.SalesTaxWithheldAtSource);
        $("#InvoiceItem_ExtraTax").val(item.ExtraTax);
        $("#InvoiceItem_FurtherTax").val(item.FurtherTax);
        $("#InvoiceItem_SroScheduleNo").val(item.SroScheduleNo);
        $("#InvoiceItem_FedPayable").val(item.FedPayable);
        $("#InvoiceItem_Discount").val(item.Discount);
        $("#InvoiceItem_SaleType").val(item.SaleType);
        $("#InvoiceItem_SroItemSerialNo").val(item.SroItemSerialNo);
        // set edit index
        $("#itemModal").data("edit-index", editIndex);
        $("#itemModal").modal("show");
    });
});
