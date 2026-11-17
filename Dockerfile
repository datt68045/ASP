# ==========================================================================
# Stage 1: Build & Publish DTGear ASP.NET Core 8.0 MVC Application
# ==========================================================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Sao chép file csproj và khôi phục các gói phụ thuộc (Restore)
COPY ["DTGear.csproj", "./"]
RUN dotnet restore "DTGear.csproj"

# Sao chép toàn bộ mã nguồn và biên dịch bản Release
COPY . .
RUN dotnet publish "DTGear.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ==========================================================================
# Stage 2: Runtime Image nhẹ và tối ưu cho triển khai (Deploy)
# ==========================================================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Đảm bảo thư mục lưu trữ đơn hàng App_Data luôn tồn tại và có quyền ghi
RUN mkdir -p /app/App_Data && chmod -R 777 /app/App_Data

COPY --from=build /app/publish .

# Cấu hình cổng mặc định 8080 (Tương thích chuẩn .NET 8, Render, Railway, Fly.io, Docker)
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

ENTRYPOINT ["dotnet", "DTGear.dll"]
