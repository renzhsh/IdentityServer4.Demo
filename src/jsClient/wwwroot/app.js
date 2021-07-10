function log() {
    var results = document.getElementById('results');
    results.innerText = '';
    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "错误：" + msg.message;
        } else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        results.innerHTML += msg + '\r\n';
    });
}

document.getElementById('login').addEventListener('click', login, false);
document.getElementById('api').addEventListener('click', api, false);
document.getElementById('logout').addEventListener('click', logout, false);

var config = {
    authority: 'http://localhost:5000',
    client_id: 'js.client',
    redirect_uri: 'http://localhost:5002/callback.html',
    response_type: 'id_token token',
    scope: 'openid profile',
    post_logout_redirect_uri: 'http://localhost:5002/index.html'
};

var manager = new Oidc.UserManager(config);


function login() {
    manager.signinRedirect();
}

function api() {
    manager.getUser().then(function (user) {
        console.dir(user)
        //var url = "http://localhost:5000/identity";
        //var xhr = new XMLHttpRequest();
        //xhr.open("GET", url);
        //xhr.onload = function () {
        //    log(xhr.status, JSON.parse(xhr.responseText));
        //}
        //xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
        //xhr.send();
    });
}

function logout() {
    manager.signoutRedirect();
}
