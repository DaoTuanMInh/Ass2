Báo cáo sau Phần 1
1. Kết quả ở Phần 1
Bạn đã hoàn thành phần nào? Phần nào còn dở? Hãy nêu một vấn đề đã khiến bạn mất nhiều thời gian và cách bạn xử lý lúc đó.
- Đã hoàn thành được các api bao gồm: 
+ P1-R01. Health check :GET /api/health
+ P1-R06. Xóa mềm :DELETE /api/work-items/{id}
+ P1-R04. Tạo công việc :POST /api/work-items
+ Các API lấy danh sách và chi tiết (GetList, GetItemDetails) cũng đã được triển khai.
- Phần đang làm dở:
+ P1-R05: Giao việc 
- Vấn đề mất nhiều thời gian: Xây dựng các điều kiện truy vấn và map dữ liệu trả về sao cho cấu trúc JSON khớp chính xác với yêu cầu của đề bài
Cách xử lý: Đọc lại thật kỹ đề bài để bóc tách từng điều kiện nhỏ sau đó research các từ khóa về linq  trong rồi kết hợp chúng lại để tìm ra điều kiện phù hợp nhất cho bài
2. Những gì đã học thêm
Chọn 2 hoặc 3 vấn đề trong bài làm. Với mỗi vấn đề, ghi nguyên nhân bạn tìm được, tài liệu đã đọc và điều bạn hiểu khác đi sau khi xem lại.
- Việc khi nào cần tạo DTO và khi nào không: Điều hiểu ra: lúc mới tiếp xúc vẫn phân vân giữa việc trả về trực tiếp model và tạo DTO sau khi tìm hiểu và áp dụng, em nhận ra nếu dùng thẳng Entity sẽ dễ làm lộ các trường dữ liệu không cần thiết và gây lỗi tham chiếu vòng trong JSON. Việc tạo các DTO như AddWorkItem, WorkItemFilterDto, ItemDetailsDto giúp xác được chính xác dữ liệu nhận vào và trả về , tách biệt hoàn toàn với cấu trúc bảng dưới DB
- Sử dụng .Include() :Khi truy vấn danh sách WorkItems, nếu muốn lấy thêm thông tin từ bảng liên quan như projects hay developers để lấy code và tên, nếu không dùng .Include() thì sẽ bị lỗi null hoặc hệ thống phải query vào DB quá nhiều lần. Sử dụng .Include(w => w.Project) giúp Entity thực hiện phép JOIN các bảng ngay trong một câu lệnh SQL duy nhất và lấy được đầy đủ object liên quan

3. Kinh nghiệm rút ra
Nếu làm lại Phần 1, bạn sẽ thay đổi điều gì trong cách đọc đề, chia thời gian, tổ chức code hoặc kiểm tra kết quả? Nêu lý do
- Khi làm lại sẽ cần đọc lướt toàn bộ đề để nắm tổng thể các luồng trước đưa ra phương án tốt nhất, sau đó mới bắt tay vào code, nếu có phần logic nào vượt quá thời gian cho phép để hiểu và giải quyết, sẽ cần bỏ qua để làm phần mới dễ hơn, đảm bảo không bị tìm hiểu sâu quá một vấn đề mà không ra kết quả 
- Về tổ chức code: Việc chia controller và service như hiện tại giúp quản lý code tốt, dễ nhìn

4. Tự đánh giá
Nêu điểm bạn làm tốt, hạn chế hiện tại và kiến thức bạn vẫn chưa hiểu chắc. Leader sẽ đối chiếu báo cáo với source code và trao đổi trực tiếp với bạn.

- Điểm làm tốt: bản thân hiện tại thấy chưa có nhiều điểm mạnh, nhưng thông qua bài này đã biết vận dụng linq để query các logic , biết áp dụng DTO khi insert nhiều bảng dữ liệu cùng lúc để đảm bảo an toàn
- Hạn chế hiện tại: Tốc độ tư duy và xử lý logic phức tạp còn chậm. Đôi khi vẫn bị rối khi xử lý các yêu cầu của đề hoặc map dữ liệu liên bảng
- Kiến thức chưa hiểu chắc: Có thể chưa thật sự thành thạo một số tính năng của entity core, cũng như kinh nghiệm tự test API khi có lỗi xảy ra vẫn còn hạn chế