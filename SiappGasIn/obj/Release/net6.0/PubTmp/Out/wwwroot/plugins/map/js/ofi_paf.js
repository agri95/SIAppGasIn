var locationMaps = {};
locationMaps["features"] = [];
$.ajax({
    url: 'MstLokasiSPBG/Retrieve',
    type: 'post',
    dataType: 'json',
    success: function (response) {

        var len = response.data.length;

        for (var i = 0; i < len; i++) {

            let temp = {
                "type": "Feature",
                "properties": { "oficina": response.data[i].namaSPBG },
                "geometry": { "type": "Point", "coordinates": [response.data[i].longitude, response.data[i].latitude] }
            }
            locationMaps.features.push(temp);
        }
    }
});

var ofi_paf = locationMaps;