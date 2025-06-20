# vancouver - Rhythm Arena
## 프로젝트 개요
1. 프로젝트 명 : Rhythm Arena
2. 개발 기간 : 3개월
3. 주요 목적 및 개발 배경
2008년 발매된 "Rhythm Heaven"게임을 베이스로 함
해당 게임의 스포츠 테마와 VR 환경의 물리 엔진을 접목시킨 Rhythm Arena를 개발

## 개발 동기 및 기획 의도
1. 기획 배경 및 문제 인식
기존 음악 게임의 박자감과 스포츠 게임의 액션을 접목시킨 게임의 부재
리듬 세상의 스포츠 테마는 행동이 제한되어 있음
따라서 자유롭게 인터렉션 할 수 있는 스포츠 리듬게임 제작 필요
2. 주요 콘셉트 및 차별점
물리 엔진을 이용한 점수 판정 시스템

## 기능 요약
- 멀티 스포츠 기반 리듬 게임 시스템
- 물리 기반 채점 시스템
- Window 기반 판정 시스템
- 로비 시스템
- 음악 연동 모듈


## 팀원 소개
- 팀장 : 조원빈
- 팀원 : 송진우, 홍성필, 버야짇, 신의성
### 역할 분배
- 조원빈 : Tabletennis Part
- 송진우 : Rhythem Judgement && Beat
- 홍성필 : Baseball Part
- 버야짇 : Boxing Part
- 신의성 : Lobby Part


## ⚙️ 설치 및 실행 가이드 (Installation & Usage Guide)

### 1. 요구 사양 (Prerequisites)
- **Unity Version**: `2022.3.5f1`
- **VR HMD**: Meta Quest 2 / Meta Quest 3 (OpenXR 기반)
- **Render Pipeline**: Universal Render Pipeline (URP)
- **VCS**: Git

### 2. 설치 및 에디터 테스트 (Installation & Editor Test)
1. Git을 사용하여 아래 명령어로 저장소를 로컬 환경에 복제(Clone)
Unity Hub에서 'Add project from disk'로 복제한 프로젝트 폴더 추가
PC에 VR HMD(헤드셋) 연결
Unity Editor에서 Assets/Scenes/Lobby.unity 씬을 열고 'Play' 버튼을 눌러 테스트 시작
3. APK 빌드 및 설치 (Build & Install for Quest)
Unity Editor 상단 메뉴에서 File > Build Settings 실행
Platform 목록에서 Android 선택 후 Switch Platform 버튼 클릭
Build 버튼을 눌러 .apk 파일 생성
생성된 .apk 파일을 SideQuest 또는 adb install 명령어를 통해 기기에 설치

## 샘플 씬
### Lobby Scene
글러브 -> 복싱씬 전환

![Image](https://github.com/user-attachments/assets/b44e3906-37a1-4791-ae12-f8a0c6e5b223)

야구공 -> BaseBall 전환

![Image](https://github.com/user-attachments/assets/789470c1-7233-4c00-a2eb-3d9faf379f2e)

탁구채 -> TableTennis 전환

![Image](https://github.com/user-attachments/assets/1c2d6274-2305-4292-8b11-168a462b0097)
### Boxing Scene
![Image](https://github.com/user-attachments/assets/d0408777-b9d3-4668-9344-c311d4a841a7)


![image](https://github.com/user-attachments/assets/9b770a69-406e-418e-b831-1e1b74dff62b)

### Table Tennis Scene

![image](https://github.com/user-attachments/assets/c8f89326-88e6-4f45-868e-aacb70a38657)

### Baseball Scene

![image](https://github.com/user-attachments/assets/deb5b35f-f28e-419e-a28d-66bb84d97356)

![image](https://github.com/user-attachments/assets/b24022d4-4810-4410-b517-79e08ab41aa6)


## License
### Lobby Scene
Code Reference - Smooth Scene Fade Transition in VR (https://youtu.be/JCyJ26cIM0Y?si=ZzKVSbSo8bEl8rDl)
Assets

-Enviroment- 
Outline - “Quick Outline” by Chris Nolet (https://assetstore.unity.com/packages/tools/particles-effects/quick-outline-115488)
Room - “Andy’s room” by Alberto (https://skfb.ly/6vAXn) License: CC Attribution
Trophy - “Championship Trophy” by IanaM (https://skfb.ly/pwUXx) License :  CC Attribution

### Table Tennis
Code Reference - None
Assets 

–Environment–
TableTennis - "Pingpong/Table tennis Lowpoly" (https://skfb.ly/oILCQ) by marvirab is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
Background - "Modular Gym" (https://skfb.ly/6VTUu) by Kristen Brown is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
Pitcher Machine - "Loaded Pitching Machine" (https://skfb.ly/oHFxR) by Alex is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/)

### Baseball
Code Reference - None
Assets

–Environment–
Ballpark
"Elevated Bleacher" (https://skfb.ly/oIoZZ) by josé serrão is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Stadium Light" (https://skfb.ly/6TXZu) by thundermind is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Baseball Field" (https://skfb.ly/6ZtHM) by Tiko is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
Bat & Ball
"Worn Baseball Ball" (https://skfb.ly/68rQq) by Alex Bes is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Baseball Bat – Pack 01" (https://skfb.ly/69JZD) by CGunwale is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
Pitcher Machine
https://sketchfab.com/3d-models/loaded-pitching-machine-11e8430608ae4a6b9c84575607660d2d {https://skfb.ly/oHFxR}

### Boxing
Code Reference - None
Assets
–Environment–
Boxing
"boxing_gloves_c4d (1)" (https://skfb.ly/puXMo) by ap-school is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
https://sketchfab.com/3d-models/ring-box-fight-pelea-9a3ba7df045d47f9b3d4106522612015

Stadium
	"Stadium Light" (https://skfb.ly/6TXZu) by thundermind is licensed under Creative 
Commons Attribution (http://creativecommons.org/licenses/by/4.0/).

Objects
"Bomberman Spawner" (https://skfb.ly/6SVtx) by zer0nim is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Melon pack" (https://skfb.ly/6SzL8) by Lassi Kaukonen is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Bomb 3D Model/ Modelo de Bomba em 3D" (https://skfb.ly/6UuSF) by Juliano.Nigro is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Explosive Barrel" (https://skfb.ly/6TzVs) by digitalghast is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
"Barrel" (https://skfb.ly/Q9SU) by Folgore is licensed under Creative Commons Attribution (http://creativecommons.org/licenses/by/4.0/).
