export function Scroll() {
    $('#small-cards').scrollTop(0);
}

export function ResizeTextArea() {
    var text = $('.description-text');

    text.click(function () {

    });

    text.each(function () {
        $(this).attr('rows', 1);
        resize($(this));
        if ($('.description-text').height() >= 290)
            $('.description-text').css('overflow', 'auto');
        else
            $('.description-text').css('overflow', 'hidden');
    });

    function resize($text) {
        $text.css('background-color', 'white');
        $text.css('height', 'auto');
        $text.css('height', $text[0].scrollHeight + 'px');
    }
}
