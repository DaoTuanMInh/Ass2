*** Hướng dẫn cách cấu hình database :
+ Tạo bảng trong  data tabase bằng các câu lệnh create, insert into bằng các câu lệnh có sẵn từ đề bài 
+ Dùng lệnh "Scaffold-DbContext "Host=10.8.0.1;Port=5432;Database=Ass2;Username=postgres;Password=arPNmdJER6m42346" " để lấy dữ liệu từ db chuyển thành entity trong couse code 
+ Vào appsetting.json để điền thông tin của ConnectionStrings.
*** Chạy ứng dụng:
+ Sau khi kết nối thành công , để kiểm tra xem đã kết nối thành công với database hay chưa thì chạy thử dự án và thử API https://localhost:7229/api/work-items/health trên post man hoặc chạy ngay trên swager, nêu hiện trạng thái 200 và mesage : thành công thì tức là đã kết nối thành công với database