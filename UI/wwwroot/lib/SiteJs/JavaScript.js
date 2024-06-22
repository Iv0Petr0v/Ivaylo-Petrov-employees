function fakeLoader() {
    return new Promise((resolve, reject) => {
        const loader = document.getElementById('loader');
        const tableContainer = document.getElementById('table-container');

        loader.style.display = 'block';
        tableContainer.style.display = 'none';

        setTimeout(() => {
            loader.style.display = 'none';
            tableContainer.style.display = 'block';
            resolve();
        }, 2000);
    });
}


function renderTable(data) {
    const container = d3.select('#table-container');

    container.selectAll('*').remove();

    const table = container.append('table').attr('class', 'table table-striped');
    const thead = table.append('thead').attr('class', 'table-dark');
    const tbody = table.append('tbody');
    const headers = ["Employee ID #1", "Employee ID #2", "Project ID", "Days worked"];
    const headerRow = thead.append('tr');

    headerRow.selectAll('th')
        .data(headers)
        .enter()
        .append('th')
        .text(d => d);

    const rows = tbody.selectAll('tr')
        .data(data)
        .enter()
        .append('tr');

    rows.selectAll('td')
        .data(d => [d.empID1, d.empID2, d.projectID, d.daysWorkedTogether])
        .enter()
        .append('td')
        .text(d => d);
}

async function uploadFile() {
    const fileInput = document.getElementById('fileInput');
    const file = fileInput.files[0];

    if (!file) {
        showMessage("Please select a file.");
        return;
    }

    const formData = new FormData();
    formData.append("formFile", file);

    try {
        const response = await fetch('http://localhost:5017/EmployeeProject/upload', {
            method: 'POST',
            body: formData
        });

        if (!response.ok) {
            throw new Error('Upload failed.');
        }

        const result = await response.json();

        fakeLoader()
            .then(() => {
                renderTable(result);
            });

        showMessage(`File uploaded successfully.`);

    } catch (error) {

        console.error('Error uploading file:', error);

        showMessage('Error uploading file. Please try again.');
    }
}

function showMessage(message) {
    const messageDiv = document.getElementById('message');
    messageDiv.textContent = message;
}
