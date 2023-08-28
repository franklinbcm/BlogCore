// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(() => {
    MaskPhone();

});
MaskPhone = () => {
    $('.csPhoneMask').inputmask([{ "mask": "(999) 999-9999" }, { "mask": "+9(999) 999-9999" }], {
        greedy: false,
        definitions: { '#': { validator: "[0-9]", cardinality: 1 } }
    });
}