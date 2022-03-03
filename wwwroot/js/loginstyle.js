var sign_in_btn;
var sign_up_btn;

var container;


function initializeLogin()
{
    sign_in_btn = document.querySelector("#sign-in-btn");
    sign_up_btn = document.querySelector("#sign-up-btn");

    container = document.querySelector(".container-main");

    console.log(container)
    console.log(sign_in_btn)
    console.log(sign_up_btn)

    sign_up_btn.addEventListener('click', () => {

        container.classList.add("sign-up-mode");
    
    });
    
    sign_in_btn.addEventListener('click', () => {
    
        container.classList.remove("sign-up-mode");
    
    });
      
}

