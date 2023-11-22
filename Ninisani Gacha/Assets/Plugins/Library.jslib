mergeInto(LibraryManager.library, {
    ConsoleLog: function (str) {
        console.log(UTF8ToString(str));
    },
    SetStorage: function(id, value) {
        localStorage.setItem(UTF8ToString(id), UTF8ToString(value));
    },
    GetStorage: function(id) {
        var returnStr = localStorage.getItem(UTF8ToString(id));
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    }
});