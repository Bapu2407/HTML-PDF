module.exports = function (callback, data) {
    var jsreport = require('jsreport-core')();

    jsreport.init().then(function () {
        return jsreport.render({
            template: {
                content: '<h1> {{:foo}}</h1>',
                engine: 'jsrender',
                recipe: 'phantom-pdf'
            },
            data: {
                foo: data
            }
        }).then(function (resp) {
            //console.log(resp.content.toString())
            var fs = require('fs');
            fs.writeFile("/tmp/test", resp.content, function (err) {
                if (err) {
                    return console.log(err);
                }
                console.log("The file was saved!");
            });
            callback(/* error */ null, resp.content.toJSON().data);
        });
    }).catch(function (e) {
        callback(/* error */ e, null);
    })
};
