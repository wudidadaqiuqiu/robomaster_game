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
- 使用[UnityStandaloneFileBrowser](https://github.com/gkngkc/UnityStandaloneFileBrowser?tab=readme-ov-file)实现跨平台文件选择
## 习惯与规范
- 所有自建文件夹和文件均以小写字母加下划线命名
- 类，结构体，枚举，函数命名使用驼峰命名法，函数
- 变量命名使用蛇形命名法
- 成员变量可以使用下划线开头来避免与形参的重复，如`_variable`
- 不区分prefab与脚本放置位置，美术资源放置在`Assets/myresources`文件夹下，场景放置在`Assets/scenes`文件夹下，其他基本放在`Assets/my_project`

## TODO
### uinty 部分
- [x] 弹丸发射与碰撞检测
- [x] 使用Netcode for Entities实现弹丸的网络同步
- [ ] 实现机器人全功能
- [ ] 实现裁判与比赛逻辑
### ros2 部分
- [ ] 接收unity发来的信息，包括裁判信息，仿真机器人自带传感器（或处理过）信息
- [ ] 发送仿真机器人控制指令信息给unity
- 具体导航及决策算法实现与升级
## 扩展资料
- [UniRx入门](https://lianbai.github.io/2019/09/23/Unity/UniRx%E5%85%A5%E9%97%A8/)

- [Entity工作流程](https://discussions.unity.com/t/convert-to-entity-script-is-not-there/257976/3)

## 问题
- [Loading Entity Scene failed because the entity header file couldn’t be resolved.](https://discussions.unity.com/t/solved-loading-entity-scene-failed-because-the-entity-header-file-couldnt-be-resolved/819677/9)
- [What is the proper way of instantiating an entity prefab with child physics bodies?](https://discussions.unity.com/t/what-is-the-proper-way-of-instantiating-an-entity-prefab-with-child-physics-bodies/910049)
- [](https://discussions.unity.com/t/how-do-i-detect-collisions/875868/3)
- ComponentLookup 仅能得到根上的实体(坐标系是全局坐标系)，不能获取到子树上的实体(坐标系是局部坐标系)
- 子对象如果含有rigid body 或 physics body组件，则在转化时会将其坐标系定为全局坐标系，导致更改父实体的坐标时，子实体不受影响