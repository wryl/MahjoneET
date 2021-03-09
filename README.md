# 大杂烩 基于各种开源项目缝合而成

主要缝合内容有: 
ET :(https://github.com/egametang/ET.git)

NaoMahjong :(https://github.com/HitomiFlower/NaoMahjong.git)

xasset热更等参考博客:(https://www.lfzxb.top/)

# ET的介绍：

牛逼不谈.最近作者更新了一大波.我也跟进了一大波.基于最新master版本:(https://github.com/egametang/ET/commit/f3195404efcdb4eb2619a1dda01ce2e2b220a35c)开发

# NaoMahjong

原项目网络模块使用Photon .改之

# xasset

好用.



# 新玩法 双人麻将对战

开启方式:MahjoneBehaviourComponent组件的IsBattleMod = true;即可

## 玩法介绍

### 开始阶段

![](https://github.com/wryl/MahjoneET/blob/mahjongBase/Tutorial/Tutorial1.png)

每个玩家选择28张牌作为牌山初始牌,系统随机20张额外牌进入牌山 总共76张牌.双方都确定后进入摸牌阶段

### 预摸牌阶段



![](https://github.com/wryl/MahjoneET/blob/mahjongBase/Tutorial/Tutorial2.png)

每个玩家查看牌山最前的三张牌,并选一张作为自己即将要摸的牌(观星) 

### 摸牌阶段

正常麻将逻辑

### 胡牌阶段

![](https://github.com/wryl/MahjoneET/blob/mahjongBase/Tutorial/Tutorial4.png)

胡牌后.该局不结束.玩家可以选择手牌/鸣牌全部进入牌河.也可以选择只将鸣牌进入牌河,然后补齐手牌重新进入预摸牌阶段.就是说玩家可以连胡.

### 流局阶段

不留局.将牌河的牌全部清空并重置牌山.



### 说明

![](https://github.com/wryl/MahjoneET/blob/mahjongBase/Tutorial/Tutorial5.png)

这个玩法可以胡很大的牌



