# KakaoBotManagerServer
- 챗봇 서비스들의 API 게이트웨이

<div align=center><h2>📚 STACKS</h2></div>

<div align=center>
  <img src="https://img.shields.io/badge/c%23-%23512BD4.svg?style=for-the-badge&logo=c-sharp&logoColor=white">
  <img src="https://img.shields.io/badge/Visual%20Studio%202022-5C2D91.svg?style=for-the-badge&logo=visual-studio&logoColor=white">
  <br/>
  <img src="https://img.shields.io/badge/Blazor%20Server-5C2D91?style=for-the-badge&logo=blazor&logoColor=white">
  <img src="https://img.shields.io/badge/redis-%23DD0031.svg?style=for-the-badge&logo=redis&logoColor=white"> 
  <br/>
  <img src="https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white"> 
  <img src="https://img.shields.io/badge/github%20actions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white">  
  <br>
</div>

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
