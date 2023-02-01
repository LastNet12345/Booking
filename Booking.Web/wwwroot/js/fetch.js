document.querySelector('#fetch').addEventListener('click', fetchCreateForm);
document.querySelector('#fetch2').addEventListener('click', fetchCreateForm2);


function fetchCreateForm() {
    fetch('https://localhost:44301/GymClasses/FetchForm', {
        method: 'GET',
        headers: {

        }
    })
        .then(res => res.text())
        .then(data => {
            createForm.innerHTML = data;
            fixValidation();
        })
        .catch(err => console.log(err));
}

async function fetchCreateForm2() {
    const res = await fetch('https://localhost:44301/GymClasses/FetchForm', { method: 'GET' });
    //Check res! res.ok
    const data = await res.text();
    createForm.innerHTML = data;
    fixValidation();
}