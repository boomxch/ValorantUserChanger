# ValorantUserChanger
Change user account by replacing "RiotClientPrivateSettings.yaml".

## アプリケーションのダウンロードリンク
[ダウンロードリンク](https://github.com/boomxch/ValorantUserChanger/raw/master/ValorantUserChanger.exe)

## 初期設定
0. アプリケーションをダウンロードする
1. 起動すると画像1のような画面が出てくる  
![画像1](https://user-images.githubusercontent.com/6965987/98113135-91a8c400-1ee6-11eb-9f43-87c97442ae4d.png)
- 赤文字で表示されているユーザはまだ設定が済んでいない状態
- 初期状態だと、ユーザ名がguidで表示されている
  - 名前を変える場合は(#カスタマイズ)の項を参照
2. `Valorantを起動`ボタンでValorantを起動する
3. 設定が済んでいないユーザで、**`サインイン状態を維持`にチェックを入れた状態で**ログインする
4. その後、ValorantUserChangerに戻り、`ユーザ更新`ボタンを押す
5. すると画像2のようにログインしたユーザ名が黒文字で表示される  
![画像2](https://user-images.githubusercontent.com/6965987/98113491-1398ed00-1ee7-11eb-85f1-9a04a80f6dfa.png)
6. `Valorantログアウト`ボタンで自動ログインを解除する
7. 2~6を全保有ユーザ分行う
8. 全てのユーザ名が黒文字で表示されるようになったら、初期設定完了

## 使用方法
- `ユーザ更新`ボタン : ユーザの自動ログインファイルを検出し、アプリケーションと同階層の`data`フォルダに格納する
- `ユーザ変更`ボタン : `ユーザ`アイコンをクリックし、ユーザを指定した状態で押すと、そのユーザの自動ログインファイルが、Riot Games用の自動ログイン用フォルダにコピーされ、そのユーザでの自動ログインが可能となる
- `Valorant起動`ボタン : Valorantを起動する
- `Valorantログアウト`ボタン : 起動中のValorantを落とし、Riot Games用の自動ログイン用フォルダに格納された自動ログイン用ファイルを削除する(ValorantUserChangerで設定したファイルが消えるわけではない)
- `ユーザ`アイコン : [カスタマイズ](#カスタマイズ)の項を参照

## カスタマイズ
- 表示ユーザ名を変えたい場合は、変えたいユーザを右クリックし、データ管理画面(画像3)にてユーザ名を記入する  
![画像3](https://user-images.githubusercontent.com/6965987/98113889-aa65a980-1ee7-11eb-9031-22edd899812a.PNG)
  - ここで変わるユーザ名は、このアプリケーション上での表示のみ