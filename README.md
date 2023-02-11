# KakaoBotManagerServer
- 챗봇 서비스들의 API 게이트웨이

## 기능
- 관리자 인증

![image](https://user-images.githubusercontent.com/66579357/218245645-a56519d7-19ed-4521-a174-cd4a334a4668.png)

- API 콜백 등록

![image](https://user-images.githubusercontent.com/66579357/218254503-80ebba43-51bf-4394-ba80-d84c25598b46.png)

## 동작
- 전체 구조 및 플로우는 KakaoBotServer 프로젝트 문서에 자세히 설명되어있음
- https://github.com/cola314/KakaoBotServer

## 환경변수
`BACKUP_FILE`  
데이터 저장 파일

`ADMIN_USERNAME`  
관리자 사용자명

`ADMIN_PASSWORD`  
관리자 비밀번호

`REDIS_SERVER`  
레디스 서버 주소

`REDIS_PORT`  
레디스 포트 번호

`MESSAGE_API_KEY`  
KakaoBotServer API 키

`WEBHOOK_SECRET`   
챗봇 서비스로 푸시를 보낼때 같이 보내는 데이터
