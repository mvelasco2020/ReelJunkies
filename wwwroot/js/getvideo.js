
function GetTVVideo(url,id,page=1,count=1) {
            fetch(`${url}?id=${id}&page=${page}&count=${count}`, {
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                }
            })
            .then(data => data.json())
                .then(r => {
                $("#trailerVideo").attr('src', `https://www.youtube.com/embed/${r.results[0].key}?autoplay=1&amp;modestbranding=1&amp;showinfo=0`);
                $('#trailerModalVideo').modal('show');
            })
                .catch(error => {
                    $("#trailerVideo").attr('src', `https://www.youtube.com/embed/NvGZ33oWieQ?autoplay=1&amp;modestbranding=1&amp;showinfo=0`);
                    $('#trailerModalVideo').modal('show');
                    console.log('Unable to get items.', error);
            })

};



function GetMovieTrailer(url, id) {
    console.log("get Movie video fired")
    fetch(`${url}${id}`, {
        headers: {
            'Content-Type': 'application/json',
            'Accept': 'application/json'
        }
    })
        .then(response => response.json())
        .then(data => {
            console.log(data)
            const start = data.indexOf("=") + 1;
            const trlId = data.substring(start, data.length);
            $("#trailerVideo").attr('src', `https://www.youtube.com/embed/${trlId}?autoplay=1&amp;modestbranding=1&amp;showinfo=0`);
            $('#trailerModalVideo').modal('show');


            $('#trailerModalVideo').on('hidden.bs.modal', function (event) {
                $("#trailerVideo").attr('src', "");
            })
        })
        .catch(error => {
            $("#trailerVideo").attr('src', `https://www.youtube.com/embed/NvGZ33oWieQ?autoplay=1&amp;modestbranding=1&amp;showinfo=0`);
            $('#trailerModalVideo').modal('show');
            console.log('Unable to get items.', error);
        });
};


