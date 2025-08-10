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
    $("#addItemBtn").on("click", function () {
        // Validate modal form
        if (!$("#itemForm").valid()) {
            return;
        }

        // Get form values
        const productText = $("#InvoiceItem_Id option:selected").text();
        const productId = $("#InvoiceItem_Id").val();
        const qty = $("#InvoiceItem_Quantity").val();
        const rate = $("#InvoiceItem_Rate").val();
        const total = (parseFloat(qty) * parseFloat(rate)).toFixed(2);

        // Append row to table
        $("#invoiceItems").append(`
            <tr data-id="${productId}">
                <td>${productText}</td>
                <td>${qty}</td>
                <td>${rate}</td>
                <td>${total}</td>
                <td>
                    <button type="button" class="btn btn-sm btn-warning edit-item">Edit</button>
                    <button type="button" class="btn btn-sm btn-danger remove-item">Remove</button>
                </td>
            </tr>
        `);

        // Close modal
        itemModal.hide();
    });

    // Edit Item
    $(document).on("click", ".edit-item", function () {
        const row = $(this).closest("tr");
        $("#InvoiceItem_Id").val(row.data("id")).trigger("change");
        $("#InvoiceItem_Quantity").val(row.find("td:eq(1)").text());
        $("#InvoiceItem_Rate").val(row.find("td:eq(2)").text());
        itemModal.show();
    });

    // Remove Item
    $(document).on("click", ".remove-item", function () {
        $(this).closest("tr").remove();
    });
});
