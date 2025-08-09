let rowIndex = 1;

// Clone row
$("#addItemBtn").click(function () {
    var $clone = $("#invoiceItemsTable tbody tr:first").clone();
    var index = $("#invoiceItemsTable tbody tr").length;

    $clone.attr("data-index", index);

    // Update input/select names and reset values
    $clone.find("input, select").each(function () {
        var name = $(this).attr("name");
        if (name) {
            var newName = name.replace(/\[\d+\]/, "[" + index + "]");
            $(this).attr("name", newName);

            // Reset value (except readonly fields)
            if (!$(this).is("[readonly]")) {
                $(this).val($(this).attr("type") === "number" ? "0" : "");
            } else {
                $(this).val("");
            }
        }
    });

    // Update validation spans
    $clone.find("span[data-valmsg-for]").each(function () {
        var valmsg = $(this).attr("data-valmsg-for");
        if (valmsg) {
            var newValmsg = valmsg.replace(/\[\d+\]/, "[" + index + "]");
            $(this).attr("data-valmsg-for", newValmsg);
        }
        $(this).text(""); // clear previous errors
    });

    // Add remove button
    $clone.find("td:last").html('<button type="button" class="btn btn-sm btn-danger removeRow"><i class="fas fa-trash"></i></button>');

    // Append to table
    $("#invoiceItemsTable tbody").append($clone);

    // Re-parse unobtrusive validation for the new row
    $.validator.unobtrusive.parse($clone);
});

// Remove row
$(document).on("click", ".removeRow", function () {
    $(this).closest("tr").remove();
});


// Auto-fill product details when product selected
$(document).on("change", ".product-select", function () {
    var $row = $(this).closest("tr");
    var productId = $(this).val();

    if (!productId) {
        $row.find(".description, .hsCode, .uom, .rate, .taxRate, .totalValue, .salesTax").val("");
        return;
    }

    $.ajax({
        url: '/Product/GetProduct/' + productId,
        type: 'GET',
        success: function (data) {
            $row.find(".description").val(data.description);
            $row.find(".hsCode").val(data.hsCode);
            $row.find(".uom").val(data.uom);
            $row.find(".rate").val(data.rate);
            $row.find(".taxRate").val(data.taxRate);

            calculateRow($row);
        },
        error: function () {
            alert("Error fetching product details.");
        }
    });
});


// Recalculate totals on qty or discount change
$(document).on("input", ".quantity, .discount", function () {
    var $row = $(this).closest("tr");
    calculateRow($row);
});

function calculateRow($row) {
    var rate = parseFloat($row.find(".rate").val()) || 0;
    var qty = parseInt($row.find(".quantity").val()) || 0;
    var discount = parseFloat($row.find(".discount").val()) || 0;
    var taxRate = parseFloat(($row.find(".taxRate").val() || "0"));

    var total = (rate * qty) - discount;
    var tax = (total * taxRate) / 100;

    $row.find(".totalValue").val(total.toFixed(2));
    $row.find(".salesTax").val(tax.toFixed(2));
}