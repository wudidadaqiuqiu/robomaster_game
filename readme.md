# Robomaster 模拟器

## 简介
Robomaster 模拟器是一个基于 Unity 的 Robomaster 比赛模拟器，用于开发、调试和测试 Robomaster 比赛中的机器人代码。

## 依赖与引用
- 版本 Unity 2022.3.41f1
- 大多数美术资源直接取自[SimulatorX](https://github.com/scutrobotlab/SimulatorX)，并且已将其放至Assets/my_resources文件夹下
- 使用UniRx进行事件处理
- 使用IngameDebugConsole实现游戏内log显示
- 使用[Netcode for GameObjects](https://docs-multiplayer.unity3d.com/netcode/current/about/)实现网络同步
- 使用[ROS-TCP-Connector](https://github.com/Unity-Technologies/ROS-TCP-Connector)实现与ROS的通信

## 习惯与规范
- 所有自建文件夹和文件均以小写字母加下划线命名
- 类，结构体，枚举，函数命名使用驼峰命名法，函数
- 变量命名使用蛇形命名法
- 成员变量可以使用下划线开头来避免与形参的重复，如`_variable`
- 不区分prefab与脚本放置位置，美术资源放置在`Assets/myresources`文件夹下，场景放置在`Assets/scenes`文件夹下，其他基本放在`Assets/my_project`

## TODO
### uinty 部分
- [ ] 弹丸发射与碰撞检测
- [ ] 使用Netcode for Entities实现弹丸的网络同步
- [ ] 实现机器人全功能
- [ ] 实现裁判与比赛逻辑
### ros2 部分
- [ ] 接收unity发来的信息，包括裁判信息，仿真机器人自带传感器（或处理过）信息
- [ ] 发送仿真机器人控制指令信息给unity
- 具体导航及决策算法实现与升级
## 扩展资料
- [UniRx入门](https://lianbai.github.io/2019/09/23/Unity/UniRx%E5%85%A5%E9%97%A8/)