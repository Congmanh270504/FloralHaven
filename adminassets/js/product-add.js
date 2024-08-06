"use strict";
Dropzone.autoDiscover = false;
var dz = null;

(function () {
    var commentEditor = document.querySelector(".comment-editor");
    var quill = commentEditor && new Quill(commentEditor, {
        modules: {
            toolbar: ".comment-toolbar"
        },
        placeholder: "Product Description",
        theme: "snow"
    });
    quill.on('text-change', function () {
        var html = quill.root.innerHTML.toString();       
        document.getElementById('description-input').value = html;
    });
    /*
    var dropzoneBasic = document.querySelector("#dropzone-basic");
    var dropzone = dropzoneBasic && new Dropzone(dropzoneBasic, {
        //url:"/Product/Upload",
        previewTemplate: '<div class="dz-preview dz-file-preview">\n<div class="dz-details">\n  <div class="dz-thumbnail">\n    <img data-dz-thumbnail>\n    <span class="dz-nopreview">No preview</span>\n    <div class="dz-success-mark"></div>\n    <div class="dz-error-mark"></div>\n    <div class="dz-error-message"><span data-dz-errormessage></span></div>\n    <div class="progress">\n      <div class="progress-bar progress-bar-primary" role="progressbar" aria-valuemin="0" aria-valuemax="100" data-dz-uploadprogress></div>\n    </div>\n  </div>\n  <div class="dz-filename" data-dz-name></div>\n  <div class="dz-size" data-dz-size></div>\n</div>\n</div>',
        autoProcessQueue: false,
        paramName: "productpictures",
        maxFilesize: 5, //mb
        maxThumbnailFilesize: 1, //mb
        maxFiles: 10,
        parallelUploads: 10,
        acceptedFiles: ".jpeg,.png,.jpg,.gif",
        uploadMultiple: true,
        addRemoveLinks: true,

        init: function () {
            dz = this;

            $("#uploadbutton").click(function () {
                dz.processQueue();
                $(this).attr("disabled", "disabled");
            });
        },
        success: function (file) {
            /*var preview = $(file.previewElement);
            preview.addClass("dz-success text-success");

            setTimeout(function () {
                dz.removeFile(file);
                $("#refreshbutton").click();
            }, 2000);
        },
        queuecomplete: function () {
            $("#uploadbutton").removeAttr("disabled");
            
            $("#refreshbutton").click();
            
        },
    }); */
    var ecommerceProductTags = document.querySelector("#ecommerce-product-tags");
    var tagify = ecommerceProductTags && new Tagify(ecommerceProductTags);
    var currentDate = new Date();
    var productDate = document.querySelector(".product-date");
    if (productDate) {
        productDate.flatpickr({
            monthSelectorType: "static",
            defaultDate: currentDate
        });
    }
})();

$(function () {
    var s, i;
    var select2Elements = $(".select2");
    if (select2Elements.length) {
        select2Elements.each(function () {
            var element = $(this);
            element.wrap('<div class="position-relative"></div>');
            element.select2({
                dropdownParent: element.parent(),
                placeholder: element.data("placeholder")
            });
        });
    }
    var formRepeater = $(".form-repeater");
    if (formRepeater.length) {
        s = 2;
        i = 1;
        formRepeater.on("submit", function (e) {
            e.preventDefault();
        });
        formRepeater.repeater({
            show: function () {
                var formControls = $(this).find(".form-control, .form-select");
                var formLabels = $(this).find(".form-label");
                formControls.each(function (index) {
                    var id = "form-repeater-" + s + "-" + i;
                    $(formControls[index]).attr("id", id);
                    $(formLabels[index]).attr("for", id);
                    i++;
                });
                s++;
                $(this).slideDown();
                $(".select2-container").remove();
                $(".select2.form-select").select2({
                    placeholder: "Placeholder text"
                });
                $(".select2-container").css("width", "100%");
                $(".form-repeater:first .form-select").select2({
                    dropdownParent: $(this).parent(),
                    placeholder: "Placeholder text"
                });
                $(".position-relative .select2").each(function () {
                    $(this).select2({
                        dropdownParent: $(this).closest(".position-relative")
                    });
                });
            }
        });
    }
});
