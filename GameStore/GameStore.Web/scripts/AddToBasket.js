function isright(obj) {
    var value = +obj.value.replace(/\D/g, '') || 0;
    var min = +obj.getAttribute('min');
    var max = +obj.getAttribute('max');
    obj.value = Math.min(max, Math.max(min, value));
}