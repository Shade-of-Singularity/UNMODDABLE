F12::ExitApp  ; Emergency Exit

global FKB := false
^Numpad6:: {
	global FKB := !FKB
	KeyWait "Numpad6"
}

Numpad5:: {
	global FKB := false
	KeyWait "Numpad5"
}

while true {
	if FKB {
		Send "{F}"
		Loop 2 {
			Sleep 5
		}
	}
}