/*!
 * bootstrap-fileinput v5.0.3
 * http://plugins.krajee.com/file-input
 *
 * Glyphicon (default) theme configuration for bootstrap-fileinput.
 *
 * Author: Kartik Visweswaran
 * Copyright: 2014 - 2019, Kartik Visweswaran, Krajee.com
 *
 * Licensed under the BSD-3-Clause
 * https://github.com/kartik-v/bootstrap-fileinput/blob/master/LICENSE.md
 */
(function ($) {
    "use strict";

    $.fn.fileinputThemes.gly = {
        fileActionSettings: {
            removeIcon: '<i class="fa fa-trash"></i>',
            uploadIcon: '<i class="fa fa-upload"></i>',
            zoomIcon: '<i class="icon-zoom-in"></i>',
            dragIcon: '<i class="fa fa-move"></i>',
            indicatorNew: '<i class="fa fa-plus-sign text-warning"></i>',
            indicatorSuccess: '<i class="fa fa-ok-sign text-success"></i>',
            indicatorError: '<i class="fa fa-exclamation-sign text-danger"></i>',
            indicatorLoading: '<i class="fa fa-hourglass text-muted"></i>'
        },
        layoutTemplates: {
            fileIcon: '<i class="fa fa-file kv-caption-icon"></i>'
        },
        previewZoomButtonIcons: {
            prev: '<i class="fa fa-triangle-left"></i>',
            next: '<i class="fa fa-triangle-right"></i>',
            toggleheader: '<i class="fa fa-resize-vertical"></i>',
            fullscreen: '<i class="fa fa-fullscreen"></i>',
            borderless: '<i class="fa fa-resize-full"></i>',
            close: '<i class="fa fa-remove"></i>'
        },
        previewFileIcon: '<i class="fa fa-file"></i>',
        browseIcon: '<i class="fa fa-folder-open"></i>&nbsp;',
        removeIcon: '<i class="fa fa-trash"></i>',
        cancelIcon: '<i class="fa fa-ban-circle"></i>',
        pauseIcon: '<i class="fa fa-pause"></i>',
        uploadIcon: '<i class="fa fa-upload"></i>',
        msgValidationErrorIcon: '<i class="fa fa-exclamation-sign"></i> '
    };
})(window.jQuery);
