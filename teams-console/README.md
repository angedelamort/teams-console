# TEAMS CONSOLE

Simple console application that connect to a team server. This is done only as a proof of concept.

## Requirements

* .Net 5.0

## TODO

* UI - Make some separation between components.
* Fix the keys so when we press left, up, down, right, backspace, etc, we de the appropriate behavior. ReadKey should not write stuff on screen.
* Make a real logger. Doesn't like the fact the we use ApplicationView to log.
* make views for rendering. This is becoming too complicated. 
    * Should be containers with position and size.
    * state will be in the view?!?!
* Add a new section that shows (how many unread channels, message, etc) with shortcut
* add a method /find to find users and start chat.
* Should be easy to switch between active chats. Maybe a status.