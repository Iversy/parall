import random
import datetime


def generate_ip():
    return f"{random.randint(0, 255)}.{random.randint(0, 255)}.{random.randint(0, 255)}.{random.randint(0, 255)}"


def generate_http_request():
    methods = ["GET", "POST", "PUT", "DELETE", "HEAD"]
    paths = ["/index.html", "/submit", "/images/logo.png", "/about", "/contact"]
    ends = ["100", "200", "300", "400", "500"]
    method = random.choice(methods)
    path = random.choice(paths)
    code = random.choice(ends)
    return f'{method} "{path} HTTP/1.1" {code}'


def generate_log_entry():
    ip = generate_ip()
    timestamp = datetime.datetime.now().strftime("%d/%b/%Y:%H:%M:%S")
    request = generate_http_request()
    return f"{ip} - - [{timestamp}] {request}"


def generate_logs(num_entries):
    logs = [generate_log_entry() for _ in range(num_entries)]
    return "\n".join(logs)


def write_logs_to_file(filename, num_entries):
    logs = generate_logs(num_entries)
    with open(filename, "w") as f:
        f.write(logs)


if __name__ == "__main__":
    num_entries = 1 << 20
    filename = "logs"
    write_logs_to_file(filename, num_entries)
    print(f"Сгенерировано {num_entries} записей в файл {filename}.")
