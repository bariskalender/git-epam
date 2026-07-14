CREATE TABLE employee (
    id INTEGER PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(200) NOT NULL UNIQUE
);

CREATE TABLE project_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(50) NOT NULL UNIQUE
);

CREATE TABLE project (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200) NOT NULL UNIQUE,
    creation_date DATE NOT NULL,
    closure_date DATE,
    project_status_id INTEGER NOT NULL,
    FOREIGN KEY (project_status_id) REFERENCES project_status(id)
);

CREATE TABLE position (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE employee_project (
    id INTEGER PRIMARY KEY,
    employee_id INTEGER NOT NULL,
    project_id INTEGER NOT NULL,
    position_id INTEGER NOT NULL,
    FOREIGN KEY (employee_id) REFERENCES employee(id),
    FOREIGN KEY (project_id) REFERENCES project(id),
    FOREIGN KEY (position_id) REFERENCES position(id)
);

CREATE TABLE task_status (
    id INTEGER PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE
);

CREATE TABLE project_task (
    id INTEGER PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description VARCHAR(500),
    deadline DATE NOT NULL,
    project_id INTEGER NOT NULL,
    employee_id INTEGER NOT NULL,
    FOREIGN KEY (project_id) REFERENCES project(id),
    FOREIGN KEY (employee_id) REFERENCES employee(id)
);

CREATE TABLE task_status_history (
    id INTEGER PRIMARY KEY,
    project_task_id INTEGER NOT NULL,
    task_status_id INTEGER NOT NULL,
    status_date DATE NOT NULL,
    employee_id INTEGER NOT NULL,
    FOREIGN KEY (project_task_id) REFERENCES project_task(id),
    FOREIGN KEY (task_status_id) REFERENCES task_status(id),
    FOREIGN KEY (employee_id) REFERENCES employee(id)
);