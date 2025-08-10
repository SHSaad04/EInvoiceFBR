$(function () {
    const itemModal = new bootstrap.Modal($("#itemModal")[0]);

    // Add Item
    $("#btnAddItem").on("click", function () {
        clearModal();
        itemModal.show();
    });

    // Edit Item (when clicking edit buttons in the table)
    $(document).on("click", ".edit-item", function () {
        fillModal($(this).data());
        itemModal.show();
    });

    function clearModal() {
        $("#ProductId").val("");
        $("#ProductName").val("");
        $("#ProductRate").val("");
        // Clear other fields if needed
    }

    function fillModal(data) {
        $("#ProductId").val(data.id);
        $("#ProductName").val(data.name);
        $("#ProductRate").val(data.rate);
        // Map other fields if needed
    }

    //Add Item click in modal
    let invoiceItems = [];
    $("#addItemBtn").on("click", function () {
        const item = {
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
            ExtraTax: parseFloat($("#InvoiceItem_ExtraTax").val()) || null,
            FurtherTax: parseFloat($("#InvoiceItem_FurtherTax").val()) || null,
            SroScheduleNo: $("#InvoiceItem_SroScheduleNo").val(),
            FedPayable: parseFloat($("#InvoiceItem_FedPayable").val()) || null,
            Discount: parseFloat($("#InvoiceItem_Discount").val()) || null,
            SaleType: $("#InvoiceItem_SaleType").val(),
            SroItemSerialNo: $("#InvoiceItem_SroItemSerialNo").val()
        };

        invoiceItems.push(item);
        renderInvoiceCards();

        $("#itemModal").modal("hide");
    });

    // Render cards
    function renderInvoiceCards() {
        const container = $("#invoiceItemsCards");
        container.empty();
        const template = $("#invoiceItemCardTemplate").html();

        invoiceItems.forEach((item, index) => {
            let cardHtml = template
                .replace(/{ProductDescription}/g, item.ProductDescription)
                .replace(/{HsCode}/g, item.HsCode)
                .replace(/{Quantity}/g, item.Quantity)
                .replace(/{UoM}/g, item.UoM)
                .replace(/{Rate}/g, item.Rate.toFixed(2))
                .replace(/{TotalValue}/g, item.TotalValue.toFixed(2))
                .replace(/{Index}/g, index);
            container.append(cardHtml);
        });
    }

    // Handle remove
    $(document).on("click", ".remove-item", function () {
        const index = $(this).data("index");
        invoiceItems.splice(index, 1);
        renderInvoiceCards();
    });

    // Handle edit
    $(document).on("click", ".edit-item", function () {
        const index = $(this).data("index");
        const item = invoiceItems[index];
        // populate modal fields with item values
        $("#InvoiceItem_HsCode").val(item.HsCode);
        $("#InvoiceItem_ProductDescription").val(item.ProductDescription);
        $("#InvoiceItem_Rate").val(item.Rate);
        $("#InvoiceItem_UoM").val(item.UoM);
        $("#InvoiceItem_Quantity").val(item.Quantity);
        $("#InvoiceItem_TotalValue").val(item.TotalValue);
        // set edit index
        $("#itemModal").data("edit-index", index);
        $("#itemModal").modal("show");
    });
});
